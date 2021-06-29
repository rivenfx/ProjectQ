. "./common.ps1"

# 移除 ProjectQ.sln 中对 Riven Framework 库的引用
$slnFilePath = "../ProjectQ.sln";

# 删除库引用 Framework
RemoveCsproj -slnFilePath $slnFilePath -libName '..\Framework\src\'

# 删除库引用 Modules
RemoveCsproj -slnFilePath $slnFilePath -libName '..\Modules\src\'


Write-Host "移除ProjectQ.sln引用已完成"

# 更新nuget引用

## framework
ReplaceCsproj -dirName 'Framework'

## modules
ReplaceCsproj -dirName 'Modules'

Write-Host "类库引用替换完成"
