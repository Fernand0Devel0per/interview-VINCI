// Types based on the DTOs from the backend
export interface Customer {
  id: string
  name: string
  email: string
}

export interface Product {
  id: string
  name: string
  price: number
}

export interface ProductInOrder {
  name: string
  price: number
}

export interface Order {
  id: string
  customerId: string
  orderDate: string
  totalAmount: number
  products: ProductInOrder[]
}

// API base URL
const API_URL = "http://localhost:8082"

// Atualizado para lidar com a estrutura de resposta da API
async function fetchApi<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
  const response = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  })

  if (!response.ok) {
    const error = await response.text()
    throw new Error(error || "An error occurred while fetching data")
  }

  const result = await response.json()

  // Verifica se a resposta tem o formato esperado
  if (result.success === false) {
    throw new Error(result.message || "A operação falhou")
  }

  // Retorna apenas os dados da propriedade 'data'
  return result.data
}

// Customer API
export const customerApi = {
  getAll: () => fetchApi<Customer[]>("/customers"),
  getById: (id: string) => fetchApi<Customer>(`/customers/${id}`),
  create: (customer: Omit<Customer, "id">) =>
    fetchApi<Customer>("/customers", {
      method: "POST",
      body: JSON.stringify(customer),
    }),
  update: (id: string, customer: Omit<Customer, "id">) =>
    fetchApi<Customer>(`/customers/${id}`, {
      method: "PUT",
      body: JSON.stringify(customer),
    }),
  delete: (id: string) =>
    fetchApi<void>(`/customers/${id}`, {
      method: "DELETE",
    }),
}

// Product API
export const productApi = {
  getAll: () => fetchApi<Product[]>("/products"),
  getById: (id: string) => fetchApi<Product>(`/products/${id}`),
  create: (product: Omit<Product, "id">) =>
    fetchApi<Product>("/products", {
      method: "POST",
      body: JSON.stringify(product),
    }),
  update: (id: string, product: Omit<Product, "id">) =>
    fetchApi<Product>(`/products/${id}`, {
      method: "PUT",
      body: JSON.stringify(product),
    }),
  delete: (id: string) =>
    fetchApi<void>(`/products/${id}`, {
      method: "DELETE",
    }),
}

// Order API
export interface CreateOrderDto {
  customerId: string
  products: { productId: string }[]
}

export const orderApi = {
  getAll: () => fetchApi<Order[]>("/orders"),
  getById: (id: string) => fetchApi<Order>(`/orders/${id}`),
  create: (order: CreateOrderDto) =>
    fetchApi<Order>("/orders", {
      method: "POST",
      body: JSON.stringify(order),
    }),
  delete: (id: string) =>
    fetchApi<void>(`/orders/${id}`, {
      method: "DELETE",
    }),
}
