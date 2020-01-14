import glob
import os
import re
import shutil
import subprocess
from win32api import GetFileVersionInfo, LOWORD, HIWORD

msbuild = r'c:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe'
nuget = r'c:\ProgramData\chocolatey\lib\NuGet.CommandLine\tools\nuget.exe'

class Builder:
    def __init__(self, project):
        self.project = project
        self.project_dir = "d:\\Projects\\" + project
        self.solution_file = self.project_dir + r'\Source\Main.sln'
        self.temp_dir = 'd:\\Temp\\' + project + '_temp'
        self.resultdll = self.project_dir + "\\Source\\_output\\Release\\" + self.project + ".dll"
        self.exclude_package_configs = list()
        self.nuget_addition_files = list()

    def cleanup_project_folder(self):
        print("Удаляю временные директории проекта")

        dirs_for_remove = ["obj", "bin", "_output", "packages"]
        for root, subdirs, files in os.walk(self.project_dir):
            for d in subdirs:
                if d.lower() in dirs_for_remove:
                    full_dir = os.path.join(root, d)
                    print( "Удаляю " + full_dir)
                    shutil.rmtree(full_dir)

        print("Удаляю временные артефакты")
        if os.path.isdir(self.temp_dir):
            shutil.rmtree(self.temp_dir)

    def nuget_run(self, command):
        print("Nuget " + command)
        p = subprocess.call([nuget, command, self.solution_file])
        return p!=1

    def build(self):
        print("Собираю проект")
        arg1 = '/t:Rebuild'
        arg2 = '/p:Configuration=Release'
        p = subprocess.call([msbuild, self.solution_file, arg1, arg2])
        return p!=1

    def find_all_package_configs(self):
        packages = []
        for root, dirs, files in os.walk(self.project_dir + "\\Source"):
            for file in files:
                if file == 'packages.config' and all(item.lower() not in root.lower() for item in self.exclude_package_configs):
                    full_dir = os.path.join(root, file)
                    print( "Добавляю " + full_dir)
                    packages.append(full_dir)
        return packages

    def extract_pack_ver(self,pack_file):
        pack_ver = []
        f = open(pack_file, "r")
        lines = f.readlines()
        f.close()
        for line in lines:
            match = re.search('<package id="(.*)" version="(.*?)"', line)
            if match:
                pack_ver.append((match.group(1), match.group(2)))
        return pack_ver


    def get_version_number(self, filename):
       info = GetFileVersionInfo (filename, "\\")
       ms = info['FileVersionMS']
       ls = info['FileVersionLS']
       return str(HIWORD (ms)) + "."  + str(LOWORD (ms)) + "."  +  str(HIWORD (ls) )+ "."  +  str(LOWORD (ls))

    def add_first_part(self, file_nuspec):
        ver = self.get_version_number(self.resultdll)
        print(ver)

        file_nuspec.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>")
        file_nuspec.write("<package xmlns=\"http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd\">")
        file_nuspec.write("<metadata>")
        file_nuspec.write("<id>ARMO-Systems." + self.project + "</id>")
        file_nuspec.write("<version>" + ver + "</version>")
        file_nuspec.write("<title>" + self.project +"</title>")
        file_nuspec.write("<authors>ARMO-Systems developers</authors>")
        file_nuspec.write("<owners>ARMO-Systems</owners>")
        file_nuspec.write("<copyright>Copyright ©2006-2019 ARMO-Systems</copyright>")
        file_nuspec.write("<requireLicenseAcceptance>false</requireLicenseAcceptance>")
        file_nuspec.write("<description>Timex Development Team Library</description>")
        file_nuspec.write("<dependencies><group targetFramework=\".NETFramework4.7.2\">")

    def add_dependency_part(self, file_nuspec):
        packages = self.find_all_package_configs()

        pack_ver = []
        for pack_file in packages:
            pack_ver.extend(self.extract_pack_ver(pack_file))

        distinct_pack = list()
        for pack in pack_ver:
            if pack not in distinct_pack:
                distinct_pack.append(pack)
        
        for pack in distinct_pack:
            file_nuspec.write("<dependency id=\"" + pack[0] + "\" version=\"" + pack[1] + "\" />")

    def add_files_part(self, file_nuspec):
        file_nuspec.write("</group></dependencies>")
        file_nuspec.write("</metadata>")
        file_nuspec.write("<files>")
        file_nuspec.write("<file src=\"" + self.resultdll + "\" target=\"lib\\net472\\" + self.project + ".dll\" />")
        for add_file in self.nuget_addition_files:
            file_nuspec.write(add_file)

    def add_last_part(self, file_nuspec):
        file_nuspec.write("</files>")
        file_nuspec.write("</package>")

    def create_nuget_package(self):
        print("Собираю nuget packet")

        nuspec_file_name = self.temp_dir + "\\" + self.project + ".nuspec"
        file_nuspec = open(nuspec_file_name, "w", encoding='utf-8')
        self.add_first_part(file_nuspec)
        self.add_dependency_part(file_nuspec)
        self.add_files_part(file_nuspec)
        self.add_last_part(file_nuspec)
        file_nuspec.close()
        
        print("Nuget Pack")
        p = subprocess.call([nuget, 'pack', nuspec_file_name, '-NonInteractive'], cwd=self.temp_dir)
        return p!=1

    def update_nuget_packeges(self):
        self.nuget_run("restore")
        self.nuget_run("update")

    def create_nuget(self):
        self.cleanup_project_folder()
        os.makedirs(self.temp_dir, exist_ok=True)
        self.nuget_run("restore")
        self.build()
        self.create_nuget_package()

