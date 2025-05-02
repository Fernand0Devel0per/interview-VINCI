'use server';

// Versão do servidor da API - para ser usada em Server Components e Server Actions
import { revalidatePath } from "next/cache"

// Types
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

export interface CreateOrderDto {
  customerId: string
  products: { productId: string }[]
}

// API base URL
const API_URL = "http://api-gateway"

// Função genérica para buscar dados da API
async function fetchApi<T>(endpoint: string, options: RequestInit = {}): Promise<T | undefined> {
  const url = `${API_URL}${endpoint}`;

  const response = await fetch(url, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
    cache: "no-store",
  });

  const rawText = await response.text();

  if (!response.ok) {
    console.error(`Erro na requisição para ${url}: ${response.status}`);
    console.error(`Corpo da resposta de erro: ${rawText}`);
    throw new Error(rawText || `Erro ${response.status}`);
  }

  if (!rawText || rawText.trim() === "") {
    console.warn(`Resposta vazia de ${url}, retornando undefined`);
    return undefined;
  }

  let result: any;
  try {
    result = JSON.parse(rawText);
  } catch (err) {
    console.error(`Falha ao fazer parse do JSON da resposta de ${url}:`, rawText);
    throw new Error("Resposta inválida da API");
  }

  if (result.success === false) {
    throw new Error(result.message || "A operação falhou");
  }

  return result.data;
}



// Customer API
export async function getCustomers(): Promise<Customer[] | undefined> {
  return fetchApi<Customer[]>("/customers")
}

export async function getCustomerById(id: string): Promise<Customer | undefined> {
  return fetchApi<Customer>(`/customers/${id}`)
}

// Product API
export async function getProducts(): Promise<Product[] | undefined> {
  return fetchApi<Product[]>("/products")
}

export async function getProductById(id: string): Promise<Product | undefined> {
  return fetchApi<Product>(`/products/${id}`)
}

// Order API
export async function getOrders(): Promise<Order[] | undefined> {
  return fetchApi<Order[]>("/orders")
}

export async function getOrderById(id: string): Promise<Order | undefined> {
  return fetchApi<Order>(`/orders/${id}`)
}


// Server Actions para mutações
export async function createCustomer(formData: FormData) {
  "use server"

  const name = formData.get("name") as string
  const email = formData.get("email") as string

  if (!name || !email) {
    throw new Error("Nome e email são obrigatórios")
  }

  await fetchApi<Customer>("/customers", {
    method: "POST",
    body: JSON.stringify({ name, email }),
  })

  revalidatePath("/customers")
}

export async function updateCustomer(id: string, formData: FormData) {
  "use server"

  const name = formData.get("name") as string
  const email = formData.get("email") as string

  if (!name || !email) {
    throw new Error("Nome e email são obrigatórios")
  }

  await fetchApi<Customer>(`/customers/${id}`, {
    method: "PUT",
    body: JSON.stringify({ name, email }),
  })

  revalidatePath("/customers")
}

export async function deleteCustomer(id: string) {
  "use server"

  await fetchApi<void>(`/customers/${id}`, {
    method: "DELETE",
  })

  revalidatePath("/customers")
}

export async function createProduct(formData: FormData) {
  "use server"

  const name = formData.get("name") as string
  const priceStr = formData.get("price") as string
  const price = Number.parseFloat(priceStr)

  if (!name || isNaN(price)) {
    throw new Error("Nome e preço válido são obrigatórios")
  }

  await fetchApi<Product>("/products", {
    method: "POST",
    body: JSON.stringify({ name, price }),
  })

  revalidatePath("/products")
}

export async function updateProduct(id: string, formData: FormData) {
  "use server"

  const name = formData.get("name") as string
  const priceStr = formData.get("price") as string
  const price = Number.parseFloat(priceStr)

  if (!name || isNaN(price)) {
    throw new Error("Nome e preço válido são obrigatórios")
  }

  await fetchApi<Product>(`/products/${id}`, {
    method: "PUT",
    body: JSON.stringify({ name, price }),
  })

  revalidatePath("/products")
}

export async function deleteProduct(id: string) {
  "use server"

  await fetchApi<void>(`/products/${id}`, {
    method: "DELETE",
  })

  revalidatePath("/products")
}

export async function createOrder(customerId: string, productIds: string[]) {
  "use server"

  if (!customerId || productIds.length === 0) {
    throw new Error("Cliente e pelo menos um produto são obrigatórios")
  }

  const orderData: CreateOrderDto = {
    customerId,
    products: productIds.map((id) => ({ productId: id })),
  }

  await fetchApi<Order>("/orders", {
    method: "POST",
    body: JSON.stringify(orderData),
  })

  revalidatePath("/orders")
}

export async function deleteOrder(id: string) {
  "use server"

  await fetchApi<void>(`/orders/${id}`, {
    method: "DELETE",
  })

  revalidatePath("/orders")
}
