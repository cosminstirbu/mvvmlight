using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using GalaSoft.MvvmLight.Views;
using UIKit;

namespace DialogServiceTest
{
    public partial class ViewController : UITableViewController
    {
        private IDialogService _newDialogService = new DialogService();
        private IDialogService _deprecatedDialogService = new DeprecatedDialogService();

        private IDialogService DialogService => _newDialogService;

        private List<TableEntry> _tableEntries = new List<TableEntry>();

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _tableEntries.Add(new TableEntry { Text = "Show Error with Message", RowSelectedDelegate = ShowErrorWithMessage });
            _tableEntries.Add(new TableEntry { Text = "Show Error with Message - default OK", RowSelectedDelegate = ShowErrorWithMessageDefaultOk });
            _tableEntries.Add(new TableEntry { Text = "Show Error with Exception", RowSelectedDelegate = ShowErrorWithException });
            _tableEntries.Add(new TableEntry { Text = "Show Error with Exception - default OK", RowSelectedDelegate = ShowErrorWithExceptionDefaultOk });
            _tableEntries.Add(new TableEntry { Text = "Show Message", RowSelectedDelegate = ShowMessage });
            _tableEntries.Add(new TableEntry { Text = "Show Message Button Title", RowSelectedDelegate = ShowMessageWithButtonTitle });
            _tableEntries.Add(new TableEntry { Text = "Show Message - default OK", RowSelectedDelegate = ShowMessageDefaultOk });
            _tableEntries.Add(new TableEntry { Text = "Show Message Both Buttons", RowSelectedDelegate = ShowMessageBothButtons });
            _tableEntries.Add(new TableEntry { Text = "Show Message - default OK and Cancel", RowSelectedDelegate = ShowMessageDefaultOKandCancel });
            _tableEntries.Add(new TableEntry { Text = "Show Message Box", RowSelectedDelegate = ShowMessageBox });
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _tableEntries.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("Cell");
            cell.TextLabel.Text = _tableEntries[indexPath.Row].Text;

            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public override async void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            await _tableEntries[indexPath.Row].RowSelectedDelegate();
        }

        private async Task ShowErrorWithMessage()
        {
            await DialogService.ShowError("Message", "Title", "ButtonTitle", () => Console.WriteLine("Show Error With Message After Hide Callback"));

            Console.WriteLine("Show Error With Message Task Completed");
        }

        private async Task ShowErrorWithMessageDefaultOk()
        {
            await DialogService.ShowError("Message", "Title", null, () => Console.WriteLine("Show Error With Message Default Ok After Hide Callback"));

            Console.WriteLine("Show Error With Message Default Ok Task Completed");
        }

        private async Task ShowErrorWithException()
        {
            await DialogService.ShowError(new Exception("Exception Message"), "Title", "ButtonTitle", () => Console.WriteLine("Show Error With Exception After Hide Callback"));
        
            Console.WriteLine("Show Error With Exception Task Completed");
        }

        private async Task ShowErrorWithExceptionDefaultOk()
        {
            await DialogService.ShowError(new Exception("Exception Message"), "Title", null, () => Console.WriteLine("Show Error With Exception Default Ok After Hide Callback"));

            Console.WriteLine("Show Error With Exception Default Ok Task Completed");
        }

        private async Task ShowMessage()
        {
            await DialogService.ShowMessage("Message", "Title");

            Console.WriteLine("Show Message Task Completed");
        }

        private async Task ShowMessageWithButtonTitle()
        {
            await DialogService.ShowMessage("Message", "Title", "ButtonTitle", () => Console.WriteLine("Show Message With Button Title After Hide Callback"));

            Console.WriteLine("Show Message With Button Title Task Completed");
        }

        private async Task ShowMessageDefaultOk()
        {
            await DialogService.ShowMessage("Message", "Title", null, () => Console.WriteLine("Show Message Default Ok After Hide Callback"));

            Console.WriteLine("Show Message Default Ok Task Completed");
        }

        private async Task ShowMessageBothButtons()
        {
            var taskConfirmButtonPressed = await DialogService.ShowMessage("Message", "Title", "ConfirmButton", "CancelButton", (confirmButtonPressed) => Console.WriteLine($"Show Message With Buttons - confirm button pressed: {confirmButtonPressed}"));

            Console.WriteLine($"Show Message With Buttons Task Completed - confirm button pressed: {taskConfirmButtonPressed}");
        }

        private async Task ShowMessageDefaultOKandCancel()
        {
            var taskConfirmButtonPressed = await DialogService.ShowMessage("Message", "Title", null, null, (confirmButtonPressed) => Console.WriteLine($"Show Message Default Ok and Cancel - confirm button pressed: {confirmButtonPressed}"));

            Console.WriteLine($"Show Message Default Ok and Cancel Task Completed - confirm button pressed: {taskConfirmButtonPressed}");
        }

        private async Task ShowMessageBox()
        {
            await DialogService.ShowMessageBox("Message", "Title");

            Console.WriteLine("Show Message Box Task Completed");
        }

        private class TableEntry
        {
            public string Text { get; set; }
            public Func<Task> RowSelectedDelegate { get; set; }
        }
    }
}
