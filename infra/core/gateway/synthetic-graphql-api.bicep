@description('The display name of the API')
param name string

@description('The name of the API Management service')
param apimServiceName string

@description('The name of the API Management logger to use (or blank to disable)')
param apimLoggerName string

@description('The path that will be exposed by the API Management service')
param path string = 'graphql'

@description('The policy to configure.  If blank, a default policy will be used.')
param policy string = ''

@description('The GraphQL schema to install.')
param schema string

@description('The list of resolvers')
param resolvers array

@description('The list of named values to install')
param namedValues array

@description('The number of bytes in the request/response body to record for diagnostic purposes')
param logBytes int = 8192

var logSettings = {
  headers: [ 'Content-type', 'User-agent' ]
  body: { bytes: logBytes }
}

resource apimService 'Microsoft.ApiManagement/service@2022-08-01' existing = {
  name: apimServiceName
}

resource apimLogger 'Microsoft.ApiManagement/service/loggers@2022-08-01' existing = if (!empty(apimLoggerName)) {
  name: apimLoggerName
  parent: apimService
}

var realPolicy = empty(policy) ? loadTextContent('./default-policy.xml') : policy

resource graphqlApi 'Microsoft.ApiManagement/service/apis@2022-08-01' = {
  name: name
  parent: apimService
  properties: {
    path: path
    apiType: 'graphql'
    displayName: name
    protocols: [ 'https', 'wss' ]
    subscriptionRequired: false
    type: 'graphql'
  }
}

resource apiNamedValues 'Microsoft.ApiManagement/service/namedValues@2022-08-01' = [for item in namedValues: {
  name: item.name
  parent: apimService
  properties: {
    displayName: item.name
    value: item.value
  }
}]

resource graphqlSchema 'Microsoft.ApiManagement/service/apis/schemas@2022-08-01' = {
  name: 'graphql'
  parent: graphqlApi
  properties: {
    contentType: 'application/vnd.ms-azure-apim.graphql.schema'
    document: {
      value: schema
    }
  }
}

resource graphqlPolicy 'Microsoft.ApiManagement/service/apis/policies@2022-08-01' = {
  name: 'policy'
  parent: graphqlApi
  properties: {
    format: 'rawxml'
    value: realPolicy
  }
  dependsOn: [
    apiNamedValues
  ]
}

module graphqlResolver 'synthetic-graphql-resolver.bicep' = [for item in resolvers: {
  name: 'graphql-resolver-${item.name}'
  params: {
    apimServiceName: apimServiceName
    graphqlApiName: graphqlApi.name
    resolverName: item.name
    schemaType: item.type
    schemaField: item.field
    resolverPolicy: item.policy
  }
  dependsOn: [ apiNamedValues, graphqlSchema, graphqlPolicy ]
}]

resource diagnosticsPolicy 'Microsoft.ApiManagement/service/apis/diagnostics@2022-08-01' = if (!empty(apimLoggerName)) {
  name: 'applicationinsights'
  parent: graphqlApi
  properties: {
    alwaysLog: 'allErrors'
    httpCorrelationProtocol: 'W3C'
    logClientIp: true
    loggerId: apimLogger.id
    metrics: true
    verbosity: 'verbose'
    sampling: {
      samplingType: 'fixed'
      percentage: 100
    }
    frontend: {
      request: logSettings
      response: logSettings
    }
    backend: {
      request: logSettings
      response: logSettings
    }
  }
}

output serviceUrl string = '${apimService.properties.gatewayUrl}/${path}'
