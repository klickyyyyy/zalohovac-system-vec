using JSONCreator.Entities;
using JSONCreator.Helpers;
using JSONCreator.Presentation.Components;

namespace JSONCreator.Presentation.Windows
{
    public class BackupJobListWindow : BaseWindow
    {
        private List<BackupJob> _jobs;

        private Table<BackupJubRow> _jobsTable;

        private Button _addJobButton;
        private Button _editJobButton;
        private Button _removeJobButton;
        private Button _refreshButton;
        private Button _goBackButton;

        private Divider _divider;

        public BackupJobListWindow(string title, List<BackupJob> jobs, Application application, IWindow? returnWindow = null)
            : base(title, application, returnWindow)
        {
            _jobs = jobs;

            _jobsTable = new Table<BackupJubRow>();

            _addJobButton = new Button("Add");
            _removeJobButton = new Button("Remove");
            _editJobButton = new Button("Edit");
            _refreshButton = new Button("Refresh ze serveru");
            _goBackButton = new Button("Go back");

            _divider = new Divider("");

            _addJobButton.Clicked += AddButtonClicked;
            _editJobButton.Clicked += EditButtonClicked;
            _removeJobButton.Clicked += RemoveButtonClicked;
            _refreshButton.Clicked += RefreshButtonClicked;
            _goBackButton.Clicked += GoBack;

            RegisterComponent(_jobsTable);
            RegisterComponent(_addJobButton);
            RegisterComponent(_editJobButton);
            RegisterComponent(_removeJobButton);
            RegisterComponent(_divider);
            RegisterComponent(_refreshButton);
            RegisterComponent(_goBackButton);

            LoadJobs();
        }

        private void LoadJobs()
        {
            _jobsTable.Items = _jobs.Select(j => new BackupJubRow(j.Id, j.Name, j.Retention.Count, j.Retention.Size)).ToList();
        }

        private void AddButtonClicked()
        {
            BackupJob job = new BackupJob();

            IWindow window = new BackupJobWindow(_application, _jobs, job, this);
            window.Submitted += () =>
            {
                bool ok = ApiHelper.CreateJob(job);
                if (!ok)
                {
                    IWindow errWindow = new ErrorWindow("Chyba", "Nepodařilo se uložit úlohu na server.", _application, this);
                    errWindow.Show();
                    return;
                }
                RefreshFromServer();
            };
            window.Show();
        }

        private void EditButtonClicked()
        {
            if (_jobsTable.SelectedItem == null)
                return;

            int index = _jobs.FindIndex(j => j.Id == _jobsTable.SelectedItem.Id);
            if (index < 0) return;

            BackupJob job = _jobs[index];

            IWindow window = new BackupJobWindow(_application, _jobs, job, this);
            window.Submitted += () =>
            {
                bool ok = ApiHelper.UpdateJob(job.Id, job);
                if (!ok)
                {
                    IWindow errWindow = new ErrorWindow("Chyba", "Nepodařilo se upravit úlohu na serveru.", _application, this);
                    errWindow.Show();
                    return;
                }
                LoadJobs();
            };
            window.Show();
        }

        private void RemoveButtonClicked()
        {
            if (_jobsTable.SelectedItem == null)
                return;

            int index = _jobs.FindIndex(j => j.Id == _jobsTable.SelectedItem.Id);
            if (index < 0) return;

            BackupJob job = _jobs[index];

            IWindow window = new DialogWindow(
                "Smazat úlohu?",
                $"Opravdu chcete smazat {job.Name}?",
                _application,
                this);

            window.Submitted += () =>
            {
                bool ok = ApiHelper.DeleteJob(job.Id);
                if (!ok)
                {
                    IWindow errWindow = new ErrorWindow("Chyba", "Nepodařilo se smazat úlohu na serveru.", _application, this);
                    errWindow.Show();
                    return;
                }
                _jobs.RemoveAt(index);
                LoadJobs();
            };
            window.Show();
        }

        private void RefreshButtonClicked()
        {
            RefreshFromServer();
        }

        private void RefreshFromServer()
        {
            List<BackupJob> fromServer = ApiHelper.GetAllJobs();
            _jobs.Clear();
            _jobs.AddRange(fromServer);
            LoadJobs();
        }

        private void GoBack()
        {
            Close();
        }
    }
}
