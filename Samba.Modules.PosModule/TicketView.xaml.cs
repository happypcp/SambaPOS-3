﻿using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Samba.Presentation.Services.Common;

namespace Samba.Modules.PosModule
{
    /// <summary>
    /// Interaction logic for TicketView.xaml
    /// </summary>
    /// 
    [Export]
    public partial class TicketView : UserControl
    {
        private readonly GridLength _buttonColumnLenght = new GridLength(1, GridUnitType.Star);
        private readonly GridLength _ticketColumnLenght = new GridLength(4, GridUnitType.Star);
        private readonly Thickness _landscapeCommandWidth = new Thickness(0);
        private readonly Thickness _portraitCommandWidth = new Thickness(5, 0, 0, 0);
        private TicketViewModel _ticketViewModel;

        [ImportingConstructor]
        public TicketView(TicketViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            _ticketViewModel = viewModel;
            EventServiceFactory.EventService.GetEvent<GenericEvent<EventAggregator>>().Subscribe(OnEvent);
        }

        private void OnEvent(EventParameters<EventAggregator> obj)
        {
            switch (obj.Topic)
            {
                case EventTopicNames.DisableLandscape:
                    DisableLandscapeMode();
                    break;
                case EventTopicNames.EnableLandscape:
                    EnableLandscapeMode();
                    break;
            }
        }

        private void EnableLandscapeMode()
        {
            SwapColumns();
            Column1.Width = _buttonColumnLenght;
            Column2.Width = _ticketColumnLenght;
            CommandButtonsColumn.Margin = _landscapeCommandWidth;
            _ticketViewModel.RefreshLayout();
        }

        private void DisableLandscapeMode()
        {
            SwapColumns();
            Column1.Width = _ticketColumnLenght;
            Column2.Width = _buttonColumnLenght;
            CommandButtonsColumn.Margin = _portraitCommandWidth;
            _ticketViewModel.RefreshLayout();
        }

        private void SwapColumns()
        {
            var c1Items = MainGrid.Children.Cast<UIElement>().Where(x => Grid.GetColumn(x) == 0).ToList();
            var c2Items = MainGrid.Children.Cast<UIElement>().Where(x => Grid.GetColumn(x) == 1).ToList();
            c1Items.ForEach(x => Grid.SetColumn(x, 1));
            c2Items.ForEach(x => Grid.SetColumn(x, 0));

        }
    }
}
