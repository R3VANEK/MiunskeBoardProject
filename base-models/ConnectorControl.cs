using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MiunskeBoardProject.base_models
{


    /// <summary>
    /// klasa XAML, której jedynym zadaniem jest odróżniać kontroler, który jest elementem Image od innych obrazków
    /// nie ma do niego żadnej logiki, ale każdy element w xamlu, który ma tą klasę dostanie swój click Handler
    /// </summary>
    public class ConnectorControl : Image
    {
     
    }
}
