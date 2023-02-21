@description('The name of the SQL server host')
param name string

@description('The location to create host resources in')
param location string = resourceGroup().location

@description('The tags to apply to resources')
param tags object = {}

@description('The name of the database to create')
param databaseName string = ''

@secure()
@description('SQL Server administrator password')
param sqlAdminPassword string

@description('SQL Server administrator password')
param sqlAdminUserName string = 'appadmin'

resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: name
  location: location
  tags: tags
  properties: {
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    administratorLogin: sqlAdminUserName
    administratorLoginPassword: sqlAdminPassword
  }

  resource database 'databases' = {
    name: databaseName
    location: location
  }

  resource firewall 'firewallRules' = {
    name: 'Azure Services'
    properties: {
      startIpAddress: '0.0.0.1'
      endIpAddress: '255.255.255.254'
    }
  }
}

output connectionString string = 'Server=${sqlServer.properties.fullyQualifiedDomainName}; Database=${sqlServer::database.name}; User=${sqlAdminUserName}' 
output databaseName string = sqlServer::database.name
