using JSONCreator.Entities;
using JSONCreator.Presentation.Components;

namespace JSONCreator.Presentation.Windows
{
    public class BackupJobWindow : BaseWindow
    {
        private BackupJob _job;
        private TextBox _nameBox;
        private TextBox _timingBox;
        private TextBox _methodBox;
        private TextBox _sourcesBox;
        private TextBox _targetsBox;
        private TextBox _retentionCountBox;
        private TextBox _retentionSizeBox;

        public BackupJobWindow(Application application, List<BackupJob> existingJobs, BackupJob job, IWindow? returnWindow = null)
            : base("Zálohovací úloha", application, returnWindow)
        {
            _job = job;

            _nameBox = new TextBox("Název:           ", 64);
            _timingBox = new TextBox("Timing (cron):   ", 64);
            _methodBox = new TextBox("Metoda:          ", 16);
            _sourcesBox = new TextBox("Zdroje (oddělte ;):", 512);
            _targetsBox = new TextBox("Cíle (oddělte ;):  ", 512);
            _retentionCountBox = new TextBox("Počet záloh:     ", 10);
            _retentionSizeBox = new TextBox("Velikost (GB):   ", 10);
            Button saveButton = new Button("Uložit");
            Button cancelButton = new Button("Zrušit");

            // nacist aktualni hodnoty
            _nameBox.Value = job.Name;
            _timingBox.Value = job.Timing;
            _methodBox.Value = job.Method;
            _sourcesBox.Value = string.Join(";", job.Sources);
            _targetsBox.Value = string.Join(";", job.Targets);
            _retentionCountBox.Value = job.Retention.Count > 0 ? job.Retention.Count.ToString() : string.Empty;
            _retentionSizeBox.Value = job.Retention.Size > 0 ? job.Retention.Size.ToString() : string.Empty;

            RegisterComponent(_nameBox);
            RegisterComponent(_timingBox);
            RegisterComponent(_methodBox);
            RegisterComponent(_sourcesBox);
            RegisterComponent(_targetsBox);
            RegisterComponent(_retentionCountBox);
            RegisterComponent(_retentionSizeBox);
            RegisterComponent(saveButton);
            RegisterComponent(cancelButton);

            saveButton.Clicked += SaveClicked;
            cancelButton.Clicked += () => Close();
        }

        private void SaveClicked()
        {
            if (string.IsNullOrWhiteSpace(_nameBox.Value))
            {
                ShowError("Název nesmí být prázdný.");
                return;
            }

            if (!int.TryParse(_retentionCountBox.Value, out int count) || count <= 0)
            {
                ShowError("Počet záloh musí být celé číslo větší než 0.");
                return;
            }

            if (!int.TryParse(_retentionSizeBox.Value, out int size) || size <= 0)
            {
                ShowError("Velikost musí být celé číslo větší než 0.");
                return;
            }

            _job.Name = _nameBox.Value;
            _job.Timing = _timingBox.Value;
            _job.Method = _methodBox.Value;
            _job.Sources = _sourcesBox.Value.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
            _job.Targets = _targetsBox.Value.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
            _job.Retention.Count = count;
            _job.Retention.Size = size;

            Submit();
        }

        private void ShowError(string message)
        {
            IWindow errWindow = new ErrorWindow("Chyba", message, _application, this);
            errWindow.Show();
        }
    }
}
