# 移除 ProjectQ.sln 中对 Riven Framework 库的引用
$slnFilePath = "../ProjectQ.sln";
$projects = (dotnet sln $slnFilePath list)

foreach ($project in $projects)
{
    if($project.StartsWith("..\Framework\src\Riven")){
        # Write-Host $project
        dotnet sln $slnFilePath remove ("../"+$project)
    }
}
Write-Host "移除ProjectQ.sln项目已完成,按任意键结束程序!"


# 修改项目中对 中对 Riven Framework 库的引用

## 获取当前Riven库的版本
[xml]$versionPropsXml = Get-Content "../../Framework/version.props"
$version = $versionPropsXml.Project.PropertyGroup.Version
$versionStr = $version.Trim()

$reg = "ProjectReference Include=""..\\..\\..\\Framework\\.*\\(.*?)(.)csproj"""
$fileList = Get-ChildItem  '../src/' -recurse *.csproj | %{$_.FullName}
foreach($file in $fileList){
    # 读取csporj的内容
    $projContent = Get-Content "$file" -Raw
    $resultProjContent = $projContent.Clone()
    # 正则匹配
    $matches = [System.Text.RegularExpressions.Regex]::Matches($projContent, $reg)
    # 循环正则匹配结果i
    foreach ($match in $matches) {
        # 库名
        $libName = $match.Groups[1].Value.ToString()
        # 新的nuget引用
        $newVal = "PackageReference Include=" + '"' + "$libName" + '"' + " Version=" + '"' + "$versionStr" + '"'
        # 替换csproj的内容
        $matchVal =  $match.Value.ToString()
        $resultProjContent = $resultProjContent.Replace("$matchVal","$newVal")

        Write-Host "$libName  $versionStr"
    }

    # 写入csproj
    Set-Content -Encoding "UTF8NoBOM" -Path "$file" -Value $resultProjContent
}

Write-Host "类库引用替换完成,按任意键结束程序!"

[System.Console]::ReadLine()
