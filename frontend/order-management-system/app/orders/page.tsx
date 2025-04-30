"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { type Order, type Customer, type Product, orderApi, customerApi, productApi } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Checkbox } from "@/components/ui/checkbox"
import { Plus, Trash2, Eye } from "lucide-react"
import { useToast } from "@/components/ui/use-toast"
import Link from "next/link"

export default function OrdersPage() {
  const [orders, setOrders] = useState<Order[]>([])
  const [customers, setCustomers] = useState<Customer[]>([])
  const [products, setProducts] = useState<Product[]>([])
  const [loading, setLoading] = useState(true)
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false)
  const [formData, setFormData] = useState({
    customerId: "",
    selectedProducts: [] as string[],
  })
  const { toast } = useToast()

  // Add a new state for customer filter
  const [customerFilter, setCustomerFilter] = useState<string>("")

  useEffect(() => {
    fetchOrders()
    fetchCustomers()
    fetchProducts()
  }, [])

  const fetchOrders = async () => {
    try {
      setLoading(true)
      const data = await orderApi.getAll()
      setOrders(data)
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to fetch orders",
        variant: "destructive",
      })
    } finally {
      setLoading(false)
    }
  }

  const fetchCustomers = async () => {
    try {
      const data = await customerApi.getAll()
      setCustomers(data)
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to fetch customers",
        variant: "destructive",
      })
    }
  }

  const fetchProducts = async () => {
    try {
      const data = await productApi.getAll()
      setProducts(data)
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to fetch products",
        variant: "destructive",
      })
    }
  }

  const handleCustomerChange = (value: string) => {
    setFormData((prev) => ({ ...prev, customerId: value }))
  }

  const handleProductToggle = (productId: string) => {
    setFormData((prev) => {
      const selectedProducts = prev.selectedProducts.includes(productId)
        ? prev.selectedProducts.filter((id) => id !== productId)
        : [...prev.selectedProducts, productId]
      return { ...prev, selectedProducts }
    })
  }

  const handleCreateOrder = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!formData.customerId || formData.selectedProducts.length === 0) {
      toast({
        title: "Error",
        description: "Please select a customer and at least one product",
        variant: "destructive",
      })
      return
    }

    try {
      await orderApi.create({
        customerId: formData.customerId,
        products: formData.selectedProducts.map((productId) => ({ productId })),
      })
      setIsCreateDialogOpen(false)
      setFormData({ customerId: "", selectedProducts: [] })
      toast({
        title: "Success",
        description: "Order created successfully",
      })
      fetchOrders()
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to create order",
        variant: "destructive",
      })
    }
  }

  const handleDeleteOrder = async (id: string) => {
    if (!confirm("Are you sure you want to delete this order?")) return

    try {
      await orderApi.delete(id)
      toast({
        title: "Success",
        description: "Order deleted successfully",
      })
      fetchOrders()
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete order",
        variant: "destructive",
      })
    }
  }

  const getCustomerName = (customerId: string) => {
    const customer = customers.find((c) => c.id === customerId)
    return customer ? customer.name : "Unknown Customer"
  }

  // Add this function after the other handler functions
  const handleCustomerFilterChange = (value: string) => {
    setCustomerFilter(value)
  }

  // Modify the orders display to filter by customer
  const filteredOrders = customerFilter ? orders.filter((order) => order.customerId === customerFilter) : orders

  return (
    <div className="space-y-4">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold tracking-tight">Orders</h1>
        <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
          <DialogTrigger asChild>
            <Button>
              <Plus className="mr-2 h-4 w-4" />
              Create Order
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[500px]">
            <DialogHeader>
              <DialogTitle>Create Order</DialogTitle>
              <DialogDescription>Create a new order by selecting a customer and products</DialogDescription>
            </DialogHeader>
            <form onSubmit={handleCreateOrder}>
              <div className="grid gap-4 py-4">
                <div className="grid gap-2">
                  <Label htmlFor="customer">Customer</Label>
                  <Select value={formData.customerId} onValueChange={handleCustomerChange}>
                    <SelectTrigger>
                      <SelectValue placeholder="Select a customer" />
                    </SelectTrigger>
                    <SelectContent>
                      {customers.map((customer) => (
                        <SelectItem key={customer.id} value={customer.id}>
                          {customer.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="grid gap-2">
                  <Label>Products</Label>
                  <div className="border rounded-md p-4 max-h-[200px] overflow-y-auto">
                    {products.length === 0 ? (
                      <p className="text-sm text-muted-foreground">No products available</p>
                    ) : (
                      products.map((product) => (
                        <div key={product.id} className="flex items-center space-x-2 py-2">
                          <Checkbox
                            id={`product-${product.id}`}
                            checked={formData.selectedProducts.includes(product.id)}
                            onCheckedChange={() => handleProductToggle(product.id)}
                          />
                          <Label htmlFor={`product-${product.id}`} className="flex-1">
                            {product.name} - ${product.price.toFixed(2)}
                          </Label>
                        </div>
                      ))
                    )}
                  </div>
                </div>
              </div>
              <DialogFooter>
                <Button type="submit">Create Order</Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Order List</CardTitle>
        </CardHeader>
        <div className="px-6 py-2 border-t flex items-center gap-4">
          <Label htmlFor="customerFilter" className="text-sm font-medium">
            Filtrar por cliente:
          </Label>
          <Select value={customerFilter} onValueChange={handleCustomerFilterChange}>
            <SelectTrigger id="customerFilter" className="w-[250px]">
              <SelectValue placeholder="Todos os clientes" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">Todos os clientes</SelectItem>
              {customers.map((customer) => (
                <SelectItem key={customer.id} value={customer.id}>
                  {customer.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
          {customerFilter && (
            <Button variant="ghost" size="sm" onClick={() => setCustomerFilter("")}>
              Limpar filtro
            </Button>
          )}
        </div>
        <CardContent>
          {loading ? (
            <div className="text-center py-4">Loading...</div>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Order ID</TableHead>
                  <TableHead>Customer</TableHead>
                  <TableHead>Date</TableHead>
                  <TableHead>Total Amount</TableHead>
                  <TableHead>Products</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredOrders.length === 0 ? (
                  <TableRow>
                    <TableCell colSpan={6} className="text-center">
                      {customerFilter ? "Nenhum pedido encontrado para este cliente" : "Nenhum pedido encontrado"}
                    </TableCell>
                  </TableRow>
                ) : (
                  filteredOrders.map((order) => (
                    <TableRow key={order.id}>
                      <TableCell className="font-medium">{order.id.substring(0, 8)}...</TableCell>
                      <TableCell>{getCustomerName(order.customerId)}</TableCell>
                      <TableCell>{new Date(order.orderDate).toLocaleDateString()}</TableCell>
                      <TableCell>${order.totalAmount.toFixed(2)}</TableCell>
                      <TableCell>{order.products.length} items</TableCell>
                      <TableCell className="text-right">
                        <Link href={`/orders/${order.id}`}>
                          <Button variant="ghost" size="icon">
                            <Eye className="h-4 w-4" />
                          </Button>
                        </Link>
                        <Button variant="ghost" size="icon" onClick={() => handleDeleteOrder(order.id)}>
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </TableCell>
                    </TableRow>
                  ))
                )}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
