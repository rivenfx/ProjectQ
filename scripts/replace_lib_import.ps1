# 移除 ProjectQ.sln 中对 Riven Framework 库的引用
# $slnFilePath = "../ProjectQ.sln";
# $projects = (dotnet sln $slnFilePath list)

# foreach ($project in $projects)
# {
#     if($project.StartsWith("..\Framework\src\Riven")){
#         # Write-Host $project
#         dotnet sln $slnFilePath remove ("../"+$project)
#     }
# }

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
        $newVal = "ProjectReference Include=" + '"' + "$libName" + '"' + " Version=" + '"' + "$versionStr" + '"'
        #   " Version=""$versionStr"" "
        # 替换csproj的内容
        $matchVal =  $match.Value.ToString()
        Write-Host $matchVal
        Write-Host $newVal

        $resultProjContent = [System.Text.RegularExpressions.Regex]::Replace("$resultProjContent", "$matchVal", "$newVal")
        # $tmpProjContent = ($resultProjContent -Replace "$matchVal","$newRef")
        # $resultProjContent =  $tmpProjContent
    }

    # 写入csproj
    # Set-Content -Encoding "UTF8NoBOM" -Path $file -Value $resultProjContent
}