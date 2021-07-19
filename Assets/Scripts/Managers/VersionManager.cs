using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This class intended to hold version related information. This information
/// will be used to prevent old versions to run. If a version was set as obsolete
/// from the server side, system will not allow it to run
/// 
/// </summary>
static class VersionManager
{
    static int currentVersionMajor = 0;
    static int currentVersionMinor = 1;

    public static string GetVersion()
    {
        return currentVersionMajor + "." + currentVersionMinor;
    }
}
