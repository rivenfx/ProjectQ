
# 替换csproj类库引用
function ReplaceCsproj($dirName) {
    # 获取库版本号
    [xml]$versionPropsXml = Get-Content "../../$dirName/version.props"
    $version = $versionPropsXml.Project.PropertyGroup.Version
    $versionStr = $version.Trim()

    # 正则表达式
    $reg = 'ProjectReference Include=""..\\..\\..\\$dirName\\.*\\(.*?)(.)csproj"'

    # 所有的csproj文件
    $fileList = Get-ChildItem  '../src/' -recurse *.csproj | % { $_.FullName }

    # 遍历替换csproj文件内容
    foreach ($file in $fileList) {
        # 读取csporj的内容
        $projContent = Get-Content "$file" -Raw
        $resultProjContent = $projContent.Clone()
        # 正则匹配
        $matches = [System.Text.RegularExpressions.Regex]::Matches($projContent, $reg)

        # 循环正则匹配替换引用
        foreach ($match in $matches) {
            # 库名
            $libName = $match.Groups[1].Value.ToString()
            # 要替换的内容
            $newVal = 'ProjectReference Include=' + '"' + "$libName" + '"' + ' Version=' + '"' + '$(Riven' + $dirName + 'Version)' + '"'
            # 被替换csproj的内容
            $matchVal = $match.Value.ToString()
            # 替换csproj内容
            $resultProjContent = $resultProjContent.Replace("$matchVal", "$newVal")
            # 打印类库信息
            Write-Host "$libName  $versionStr"
        }

        # 写入csproj
        Set-Content -Encoding "UTF8NoBOM" -Path "$file" -Value $resultProjContent
    }
  

    # 更新库版本
    ReplaceVersion -dirName $dirName -version $versionStr
}

# 替换类库版本
function ReplaceVersion ($dirName, $version) {
    # 版本文件路径
    $file = "../version.props"

    ## 获取当前的版本
    [xml]$versionPropsXml = Get-Content -Encoding "UTF8NoBOM" -Path "$file"

    # 版本配置名称
    $name = '$(Riven' + $dirName + 'Version)' + '"'

    # 设置新版本
    $versionPropsXml.Project.PropertyGroup[$name].InnerText = $version

    # 写入版本文件
    $versionPropsXml.Save($file)
}

# 移除项目
function RemoveCsproj ($slnFilePath, $libName) {
    $projects = (dotnet sln $slnFilePath list)
    foreach ($project in $projects) {
        if ($project.StartsWith("$libName")) {
            # 删除库引用
            dotnet sln $slnFilePath remove $project
        }
    }
}