@echo off
pushd %~dp0

set PUML_GEN=.\PlantUmlClassDiagramGenerator\PlantUmlClassDiagramGenerator.exe
set VM_SOURCE="..\ReactiveBingViewer\ViewModels"
set VM_DEST=".\ClassDiagrams\ViewModels"
set M_SOURCE="..\ReactiveBingViewer\Models"
set M_DEST=".\ClassDiagrams\Models"
set NOTIFIERS_SOURCE="..\ReactiveBingViewer\Notifiers"
set NOTIFIERS_DEST=".\ClassDiagrams\Notifiers"

%PUML_GEN% %VM_SOURCE% %VM_DEST%
%PUML_GEN% %M_SOURCE% %M_DEST%
%PUML_GEN% %NOTIFIERS_SOURCE% %NOTIFIERS_DEST%

popd
