<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <appSettings>
        <add key="Log2File" value="true" />
        <add key="UseNewRef" value="false" />
        <add key="RefRemoteStationUrl" value="tcp://10.40.40.242:9633/RefRemoteStation"/>
        <add key="RefRemoteStationUrl2" value="tcp://10.40.40.243:9633/RefRemoteStation"/>
        <add key="RemoteRefURL" value="tcp://10.40.40.29:8098/CallGetRefFunction" />
    </appSettings>
    <system.webServer>
        <handlers>
            <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
        </handlers>
        <aspNetCore processPath=".\RefService.exe" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" />
    </system.webServer>
</configuration>
<!--ProjectGuid: EF29B99B-8F90-4D97-BFCC-5D338AC8AC74-->