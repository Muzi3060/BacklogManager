using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NuGet.Versioning;
namespace BacklogManager.Configuracao;

public static class VersaoAplicativo
{
    public static string Atual => typeof(VersaoAplicativo).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    public static bool TentarLer(string? tag,out NuGetVersion? versao)
    {
        versao=null;
        if(string.IsNullOrEmpty(tag) || tag.Length>120)return false;
        if(tag.StartsWith('v'))tag=tag[1..];
        if(!Regex.IsMatch(tag,@"\A(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?(\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?\z",RegexOptions.CultureInvariant,TimeSpan.FromMilliseconds(100)))return false;
        var prerelease=tag.Split('+')[0].Split('-',2);
        if(prerelease.Length==2 && prerelease[1].Split('.').Any(p=>p.Length>1 && p.All(char.IsAsciiDigit) && p[0]=='0'))return false;
        return NuGetVersion.TryParse(tag,out versao);
    }
}
