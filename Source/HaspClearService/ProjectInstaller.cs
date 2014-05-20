using System.ComponentModel;
using System.Configuration.Install;

namespace ArmoSystems.ArmoGet.HaspClearService
{
    [RunInstaller( true )]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}