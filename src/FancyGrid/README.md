                                                                      
,------.                               ,----.          ,--.   ,--.    
|  .---',--,--.,--,--,  ,---.,--. ,--.'  .-./   ,--.--.`--' ,-|  |    
|  `--,' ,-.  ||      \| .--' \  '  / |  | .---.|  .--',--.' .-. |    
|  |`  \ '-'  ||  ||  |\ `--.  \   '  '  '--'  ||  |   |  |\ `-' |    
`--'    `--`--'`--''--' `---'.-'  /    `------' `--'   `--' `---'     
                             `---'                                    

#####################################
# Putting this control in your view #
#####################################

Add BD.UI.WPF as a reference.
Add `xmlns:bdwpf="clr-namespace:BD.UI.WPF;assembly=BD.UI.WPF"` to your XAML namespaces.
Add

	`<bdwpf:FilteringDataGrid ItemsSource="{Binding SomeCollection}" />`

where you want your fancygrid to go. 

It will be bound to the SomeCollection property on your view model with pure microsoft magic.

##############################################
# If your grid looks just like a normal grid #
##############################################

Add
		`
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/BD.UI.WPF;component/Themes/Generic.xaml"></ResourceDictionary>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
		`
To your `<Application.Resources>` in your `App.xaml`

I'm working on making it so you don't have to do this.

#####################################
# To hide columns or rename headers #
#####################################
`
[DisplayName("A Lovely Foo")]
public int Foo {get;set;} // Will show in grid as A Lovely Foo

public int Bar {get;set;} // Will show in grid as Bar

[Browseable(false)]
public string Fizz {get;set;} // Won't Show in grid.
`
You can also use all the attributes in `System.ComponentModel`

#############################################################################
# To access items that are currently in the filter, and in the sorted order #
#############################################################################

Just grab the `Items` property

#########################################
# To access all items in original order #
#########################################

Grab the `ItemsSource` Property. If `ItemsSource` is null, try `DataContext`. 

###############################################
# Control Created by owen.johnson@pioneer.com #
###############################################