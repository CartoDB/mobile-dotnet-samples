import sys
from xml.dom import minidom

# Used to automate .apk build process on Jenkins and upload to HockeyApp
# Nuget does not feature a decent internal tool to update packages
# Changes the version in a project's .csproj file and its packages.config,

# Arguments: 
# (1) .csproj path
# (2) packages.config path
# (3) version name/number (semantic)
# (4) platform

# Sample usage:
# python update-package.py AdvancedMap.Droid/AdvancedMap.Droid.csproj AdvancedMap.Droid/packages.config 4.0.0-pre-197

# Disclaimer:
# This goes hand-in-hand with nuget.exe's package restore, 
# else the numbers will be updated, but packages will not be restored.
# Intended to work with CARTO's mobile-dotnet-samples and has not been tested on other projects.
# Use at your own risk.
# Additionally, it changes the spacing of your files

SDKNAME = "CartoMobileSDK." + sys.argv[4]

csprojFile = sys.argv[1]
packageFile = sys.argv[2]
version = sys.argv[3]

print "Updating: " + csprojFile;

xmldoc = minidom.parse(csprojFile)
references = xmldoc.getElementsByTagName("Reference")

found = False;

for reference in references:
    if reference.attributes["Include"].value == SDKNAME:
    	
    	hintpath = reference.getElementsByTagName("HintPath")[0];
    	split = hintpath.firstChild.nodeValue.split("\\")

    	base = split[0] + "\\" + split[1] + "\\"
    	end = "\\" + split[3] + "\\" + split[4] + "\\" + split[5]
    	hintpath.firstChild.nodeValue = base + SDKNAME + "." + version + end

    	found = True;

if not found:
	print "Couldn't find SDK reference in .csproj"
	quit()

xmldoc.writexml(open(csprojFile, 'w'), indent="", addindent="", newl='\n')

print "Updating: " + packageFile;

xmldoc = minidom.parse(packageFile)
packages = xmldoc.getElementsByTagName("package")

found = False;

for package in packages:
	if package.attributes["id"].value == SDKNAME:
		package.attributes["version"] = version
		found = True

if not found:
	print "Couldn't find SDK reference in packages.config"
	quit()

xmldoc.writexml(open(packageFile, 'w'), indent="", addindent="", newl='\n')











