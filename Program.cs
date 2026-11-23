using PawHaven.UI;

namespace PawHaven;

static class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new RoleSelectionForm());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "PawHaven error");
        }
    }
}