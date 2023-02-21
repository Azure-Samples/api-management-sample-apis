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

var actualDatabaseName = !empty(databaseName) ? databaseName : 'Samples'

module sqlserver '../core/database/sqlserver.bicep' = {
  name: 'sqlserver'
  params: {
    name: name
    location: location
    tags: tags
    databaseName: actualDatabaseName
    sqlAdminPassword: sqlAdminPassword
  }
}

output connectionString string = sqlserver.outputs.connectionString
output databaseName string = sqlserver.outputs.databaseName
