param name string
param location string
param tags object = {}
param appSettings object = {}
param connectionStrings object = {}

param applicationInsightsName string = ''
param appServicePlanId string
param apiManagementServiceName string = ''
param apiManagementLoggerName string = ''
param path string = 'todo-graphql'

module apiService '../core/host/appservice.bicep' = {
  name: name
  params: {
    name: name
    location: location
    tags: tags
    appCommandLine: ''
    applicationInsightsName: applicationInsightsName
    appServicePlanId: appServicePlanId
    appSettings: appSettings
    connectionStrings: connectionStrings
    runtimeName: 'dotnetcore'
    runtimeVersion: '6.0'
    scmDoBuildDuringDeployment: false
    useManagedIdentity: true
  }
}

module graphqlApiDefinition '../core/gateway/graphql-api.bicep' = if (!empty(apiManagementServiceName)) {
  name: 'todo-graphql-api-definition'
  params: {
    name: 'todo-graphql'
    apimServiceName: apiManagementServiceName
    apimLoggerName: apiManagementLoggerName
    path: path
    serviceUrl: apiService.outputs.uri
    policy: loadTextContent('../../src/ApiManagement/TodoGraphQLApi/policy.xml')
    schema: loadTextContent('../../src/ApiManagement/TodoGraphQLApi/schema.graphql')
  }
}

output serviceUri string = apiService.outputs.uri
output gatewayUri string = graphqlApiDefinition.outputs.serviceUrl
output servicePrincipalId string = apiService.outputs.servicePrincipalId
