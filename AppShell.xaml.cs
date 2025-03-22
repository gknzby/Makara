namespace Makara;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Register routes for navigation
        //Routing.RegisterRoute("wordpick", typeof(Views.WordPickPage));
        //Routing.RegisterRoute("datamigrate", typeof(Views.DataMigratePage));


        Routing.RegisterRoute("itemspage", typeof(Views.ItemsPage));
    }
}
