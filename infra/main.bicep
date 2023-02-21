// ---------------------------------------------------------------------------------------------
//
//  Infrastructure for spinning up on API services for testing GraphQL on API Management
//  
//  Copyright (C) Microsoft, Inc. All Rights Reserved
//  Licensed under the MIT License
//
// ---------------------------------------------------------------------------------------------
targetScope = 'subscription'

// ---------------------------------------------------------------------------------------------
//  Parameters - these are handled by the Azure Developer CLI
// ---------------------------------------------------------------------------------------------
@minLength(1)
@maxLength(64)
@description('Name of the the environment which is used to generate a short unique hash used in all resources.')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

@secure()
@description('SQL Server administrator password')
param sqlAdminPassword string

// ---------------------------------------------------------------------------------------------
//  Optional Parameters
//    Each resource has an optional override for the default azd resource naming conventions.
//    Update the main.parameters.json file to specify them:
//
//    "webServiceName": {
//      "value": "my-web-service"
//    }
// ---------------------------------------------------------------------------------------------

// Supporting services
param applicationInsightsDashboardName string = ''
param applicationInsightsName string = ''
param appServicePlanName string = ''
param sqlServerName string = ''
param sqlDatabaseName string = ''
param logAnalyticsName string = ''
param redisCacheServiceName string = ''
param resourceGroupName string = ''

// Underlying API Service Names
param starwarsRestServiceName string = ''
param todoRestServiceName string = ''

// Web applications
param todoReactRestWebServiceName string = ''

// API Management instance
param apiManagementServiceName string = ''

// Location over-rides.  These are provided for when the service in question is not available in all regions.
param appInsightsLocationName string = ''
param staticSitesLocationName string = ''

// ---------------------------------------------------------------------------------------------
//  Variables
//    These should not need to be touched.
// ---------------------------------------------------------------------------------------------
var abbrs = loadJsonContent('./abbreviations.json')
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = { 'azd-env-name': environmentName }

// ---------------------------------------------------------------------------------------------
//  RESOURCE GROUP
// ---------------------------------------------------------------------------------------------
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: !empty(resourceGroupName) ? resourceGroupName : '${abbrs.resourcesResourceGroups}${environmentName}'
  location: location
  tags: tags
}

// ---------------------------------------------------------------------------------------------
//  MONITORING (Azure Monitor, Application Insights)
// ---------------------------------------------------------------------------------------------
module monitoring './core/monitor/monitoring.bicep' = {
  name: 'monitoring'
  scope: rg
  params: {
    location: !empty(appInsightsLocationName) ? appInsightsLocationName : location
    tags: tags
    logAnalyticsName: !empty(logAnalyticsName) ? logAnalyticsName : '${abbrs.operationalInsightsWorkspaces}${resourceToken}'
    applicationInsightsName: !empty(applicationInsightsName) ? applicationInsightsName : '${abbrs.insightsComponents}${resourceToken}'
    applicationInsightsDashboardName: !empty(applicationInsightsDashboardName) ? applicationInsightsDashboardName : '${abbrs.portalDashboards}${resourceToken}'
  }
}

// ---------------------------------------------------------------------------------------------
//  Database (Cosmos DB)
// ---------------------------------------------------------------------------------------------
module database './app/database.bicep' = {
  name: 'database'
  scope: rg
  params: {
    name: !empty(sqlServerName) ? sqlServerName : '${abbrs.sqlServers}${resourceToken}'
    databaseName: sqlDatabaseName
    location: location
    tags: tags
    sqlAdminPassword: sqlAdminPassword
  }
}

// ---------------------------------------------------------------------------------------------
//  API Services (App Services)
// ---------------------------------------------------------------------------------------------
module appServicePlan './core/host/appserviceplan.bicep' = {
  name: 'appserviceplan'
  scope: rg
  params: {
    name: !empty(appServicePlanName) ? appServicePlanName : '${abbrs.webServerFarms}${resourceToken}'
    location: location
    tags: tags
    sku: {
      name: 'B1'
    }
  }
}

// ---------------------------------------------------------------------------------------------
//  Redis Cache
// ---------------------------------------------------------------------------------------------
module redisCache './core/cache/redis.bicep' = {
  name: 'redis-cache'
  scope: rg
  params: {
    name: !empty(redisCacheServiceName) ? redisCacheServiceName : '${abbrs.cacheRedis}${resourceToken}'
    location: location
    tags: tags
    sku: 'Basic'
    capacity: 1
  }
}

// ---------------------------------------------------------------------------------------------
//  API Management Service
// ---------------------------------------------------------------------------------------------
module apiManagement './core/gateway/api-management.bicep' = {
  name: 'api-management'
  scope: rg
  params: {
    name: !empty(apiManagementServiceName) ? apiManagementServiceName : '${abbrs.apiManagementService}${resourceToken}'
    location: location
    tags: tags
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    sku: 'Developer'
    redisCacheServiceName: redisCache.outputs.cacheName
  }
}

// ---------------------------------------------------------------------------------------------
//  API: Star Wars REST
// ---------------------------------------------------------------------------------------------
module starWarsRestApiService './app/starwars-rest-api.bicep' = {
  name: 'starwars-rest-api-service'
  scope: rg
  params: {
    name: !empty(starwarsRestServiceName) ? starwarsRestServiceName : 'starwars-rest-${resourceToken}'
    location: location
    tags: union(tags, { 'azd-service-name': 'starwars-rest' })
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    appServicePlanId: appServicePlan.outputs.id
    apiManagementServiceName: apiManagement.outputs.serviceName
    apiManagementLoggerName: apiManagement.outputs.loggerName
  }
}

// ---------------------------------------------------------------------------------------------
//  API: Todo REST
// ---------------------------------------------------------------------------------------------
module todoRestApiService './app/todo-rest-api.bicep' = {
  name: 'todo-rest-api-service'
  scope: rg
  params: {
    name: !empty(todoRestServiceName) ? todoRestServiceName : 'todo-rest-${resourceToken}'
    location: location
    tags: union(tags, { 'azd-service-name': 'todo-rest' })
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    connectionStrings: {
      DefaultConnection: {
        type: 'SQLAzure'
        value: '${database.outputs.connectionString}; Password=${sqlAdminPassword}'
      }
    }
    appServicePlanId: appServicePlan.outputs.id
    apiManagementServiceName: apiManagement.outputs.serviceName
    apiManagementLoggerName: apiManagement.outputs.loggerName
  }
}

module todoReactApp './app/todo-react-rest.bicep' = {
  name: 'todo-react-rest-app'
  scope: rg
  params: {
    name: !empty(todoReactRestWebServiceName) ? todoReactRestWebServiceName : 'todo-rest-${abbrs.webStaticSites}${resourceToken}'
    location: !empty(staticSitesLocationName) ? staticSitesLocationName : location
    tags: union(tags, { 'azd-service-name': 'todo-react-rest' })
  }
}

// ---------------------------------------------------------------------------------------------
//  OUTPUTS
//
//  These are used by Azure Developer CLI to configure deployed applications.
//
// ---------------------------------------------------------------------------------------------
output APPLICATIONINSIGHTS_CONNECTION_STRING string = monitoring.outputs.applicationInsightsConnectionString
output API_MANAGEMENT_SERVICE_URI string = apiManagement.outputs.uri
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
output STARWARS_REST_GATEWAY_URI string = starWarsRestApiService.outputs.gatewayUri
output TODO_REST_GATEWAY_URI string = todoRestApiService.outputs.gatewayUri

// Outputs for the TODO_REACT_REST app
output TODO_REACT_REST_API_BASE_URL string = todoRestApiService.outputs.gatewayUri
output TODO_REACT_REST_APPLICATIONINSIGHTS_CONNECTION_STRING string = monitoring.outputs.applicationInsightsConnectionString
output TODO_REACT_REST_WEB_BASE_URL string = todoReactApp.outputs.SERVICE_WEB_URI
