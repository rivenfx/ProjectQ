# 执行公用脚本
. ".\common.ps1"

# 切换到上级目录
Set-Location $rootPath


# 移除 ProjectQ.sln 中对 Riven Framework 库的引用
$slnFilePath = "./ProjectQ.sln"

# 删除库引用 Modules
RemoveCsproj -slnFilePath $slnFilePath -libName '..\Modules\src\'

# 删除库引用 Framework
RemoveCsproj -slnFilePath $slnFilePath -libName '..\Framework\src\'

# 打印信息
Write-Host '移除ProjectQ.sln引用已完成'






# 更新nuget引用

## modules
ReplaceCsproj -dirName 'Modules'

## framework
ReplaceCsproj -dirName 'Framework'

# 打印信息
Write-Host '类库引用替换完成'


# 切换到启动目录
Set-Location $currentPath