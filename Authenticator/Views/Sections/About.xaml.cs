﻿using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace Authenticator.Views.Sections
{
    public sealed partial class About : UserControl
    {
        public readonly string AppVersion;

        public About()
        {
            this.InitializeComponent();
            this.AppVersion = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);
        }
    }
}