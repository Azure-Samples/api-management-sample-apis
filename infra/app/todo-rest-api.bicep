param name string
param location string
param tags object = {}
param appSettings object = {}
param connectionStrings object = {}

param applicationInsightsName string = ''
param appServicePlanId string
param apiManagementServiceName string = ''
param apiManagementLoggerName string = ''
param path string = 'todo-rest'

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

module restApiDefinition '../core/gateway/rest-api.bicep' = if (!empty(apiManagementServiceName)) {
  name: 'todo-rest-api-definition'
  params: {
    name: 'todo-rest'
    apimServiceName: apiManagementServiceName
    apimLoggerName: apiManagementLoggerName
    path: path
    serviceUrl: apiService.outputs.uri
    policy: loadTextContent('../../src/ApiManagement/TodoRestApi/policy.xml')
    definition: loadTextContent('../../src/ApiManagement/TodoRestApi/swagger.json')
  }
}

output serviceUri string = apiService.outputs.uri
output gatewayUri string = restApiDefinition.outputs.serviceUrl
output servicePrincipalId string = apiService.outputs.servicePrincipalId
