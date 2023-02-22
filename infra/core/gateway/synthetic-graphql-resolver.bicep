param apimServiceName string
param graphqlApiName string
param resolverName string
param schemaType string
param schemaField string
param resolverPolicy string

resource apimService 'Microsoft.ApiManagement/service@2022-08-01' existing = {
  name: apimServiceName
}

resource graphqlApi 'Microsoft.ApiManagement/service/apis@2022-08-01' existing = {
  name: graphqlApiName
  parent: apimService
}

resource graphqlResolver 'Microsoft.ApiManagement/service/apis/resolvers@2022-08-01' = {
  name: resolverName
  parent: graphqlApi
  properties: {
    displayName: resolverName
    path: '${schemaType}/${schemaField}'
    description: 'GraphQL Resolver for ${schemaType}/${schemaField}'
  }
}

resource graphqlResolverPolicy 'Microsoft.ApiManagement/service/apis/resolvers/policies@2022-08-01' = {
  name: 'policy'
  parent: graphqlResolver
  properties: {
    format: 'rawxml'
    value: resolverPolicy
  }
}
