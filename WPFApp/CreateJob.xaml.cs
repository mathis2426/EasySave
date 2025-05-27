using ControllerModel.Jobs;
using System.Windows.Controls;


namespace WPFApp
{
    /// <summary>
    /// Logique d'interaction pour CreateJob.xaml
    /// </summary>
    public partial class CreateJob : Page
    {
        private Frame _mainFrame;
        private JobManager _jobManager;

        public CreateJob(Frame mainFrame, JobManager jobManager)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _jobManager = jobManager;

            var viewModel = new CreateJobViewModel(jobManager, mainFrame);
            this.DataContext = viewModel;
        }
    }

}
