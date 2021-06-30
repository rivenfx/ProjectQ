# 路径
$currentPath = $(Get-Location).Path # 当前启动目录
$rootPath = Join-Path  $currentPath '../' # 项目根目录

# 替换csproj类库引用
function ReplaceCsproj($dirName) {
    # 获取库版本号
    [xml]$versionPropsXml = Get-Content "../$dirName/version.props"
    $version = $versionPropsXml.Project.PropertyGroup.Version
    $versionStr = $version.Trim()

    # 正则表达式
    $reg = 'ProjectReference Include="..\\..\\..\\' + $dirName + '\\.*\\(.*?)(.)csproj"'
    Write-Host $reg

    # 所有的csproj文件
    $fileList = Get-ChildItem  './src/' -recurse *.csproj | % { $_.FullName }

    # 遍历替换csproj文件内容
    foreach ($file in $fileList) {
        # 读取csporj的内容
        $projContent = Get-Content  -Encoding "UTF8NoBOM" -Path "$file" -Raw
        $resultProjContent = $projContent.Clone()
        # 正则匹配
        $matches = [System.Text.RegularExpressions.Regex]::Matches($projContent, $reg)

        # 循环正则匹配替换引用
        foreach ($match in $matches) {
            # 被替换csproj的内容
            $matchVal = $match.Value.ToString()
            # 库名
            $libName = GetPackageId -csprojPath  $file -projectReferenceContent $matchVal
            # 要替换的内容
            $newVal = 'ProjectReference Include=' + '"' + "$libName" + '"' + ' Version=' + '"' + '$(Riven' + $dirName + 'Version)' + '"'
            # 替换csproj内容
            $resultProjContent = $resultProjContent.Replace("$matchVal", "$newVal")
            # 打印类库信息
            Write-Host "$libName  $versionStr"
        }

        # 写入csproj
        Set-Content -Encoding "UTF8NoBOM" -Path "$file" -Value $resultProjContent

        # xml 格式化读写
        [xml]$xmlFile = Get-Content -Encoding "UTF8NoBOM" -Path "$file" -Raw
        $xmlFile.Save($file)
    }
  

    # 更新库版本
    ReplaceVersion -dirName $dirName -version $versionStr
}

# 替换类库版本
function ReplaceVersion ($dirName, $version) {
    # 版本文件路径
    $file = Join-Path $rootPath 'version.props'

    ## 获取当前的版本
    [xml]$versionPropsXml = Get-Content -Encoding "UTF8NoBOM" -Path "$file"

    # 版本配置名称
    $name = 'Riven' + $dirName + 'Version'

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

# 获取包id
function GetPackageId ($csprojPath, $projectReferenceContent) {
    # 获取包的csproj所在相对路径
    ## 正则
    $reg = 'ProjectReference Include="(.*?)"'
    ## 匹配
    $match = [System.Text.RegularExpressions.Regex]::Match($projectReferenceContent, $reg)
    ## 包的csproj相对路径
    $libCsprojPath = $match.Groups[1].Value.ToString()

    # 引用包的csproj所在目录
    $csProjDir = ([System.IO.FileInfo]$csprojPath).Directory.FullName

    # 包csproj全路径
    $libCsprojFullPath = Join-Path $csProjDir $libCsprojPath

    # 包csproj内容
    [xml]$csprojXml = Get-Content -Encoding "UTF8NoBOM" -Path "$libCsprojFullPath"

    # 包名称
    return $csprojXml.Project.PropertyGroup.PackageId
}