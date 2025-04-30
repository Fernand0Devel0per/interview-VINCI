"use client"

import { useState, useEffect } from "react"
import { type Order, type Customer, orderApi, customerApi } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { ArrowLeft, Trash2 } from "lucide-react"
import { useToast } from "@/components/ui/use-toast"
import { useRouter } from "next/navigation"
import Link from "next/link"

export default function OrderDetailsPage({ params }: { params: { id: string } }) {
  const [order, setOrder] = useState<Order | null>(null)
  const [customer, setCustomer] = useState<Customer | null>(null)
  const [loading, setLoading] = useState(true)
  const { toast } = useToast()
  const router = useRouter()

  useEffect(() => {
    fetchOrderDetails()
  }, [params.id])

  const fetchOrderDetails = async () => {
    try {
      setLoading(true)
      const orderData = await orderApi.getById(params.id)
      setOrder(orderData)

      // Fetch customer details
      if (orderData.customerId) {
        const customerData = await customerApi.getById(orderData.customerId)
        setCustomer(customerData)
      }
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to fetch order details",
        variant: "destructive",
      })
    } finally {
      setLoading(false)
    }
  }

  const handleDeleteOrder = async () => {
    if (!confirm("Are you sure you want to delete this order?")) return

    try {
      await orderApi.delete(params.id)
      toast({
        title: "Success",
        description: "Order deleted successfully",
      })
      router.push("/orders")
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to delete order",
        variant: "destructive",
      })
    }
  }

  if (loading) {
    return <div className="text-center py-8">Loading order details...</div>
  }

  if (!order) {
    return (
      <div className="text-center py-8">
        <h1 className="text-2xl font-bold mb-4">Order not found</h1>
        <Link href="/orders">
          <Button>
            <ArrowLeft className="mr-2 h-4 w-4" />
            Back to Orders
          </Button>
        </Link>
      </div>
    )
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Link href="/orders">
            <Button variant="outline" size="icon">
              <ArrowLeft className="h-4 w-4" />
            </Button>
          </Link>
          <h1 className="text-3xl font-bold tracking-tight">Order Details</h1>
        </div>
        <Button variant="destructive" onClick={handleDeleteOrder}>
          <Trash2 className="mr-2 h-4 w-4" />
          Delete Order
        </Button>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Order Information</CardTitle>
          </CardHeader>
          <CardContent>
            <dl className="grid grid-cols-2 gap-4">
              <div>
                <dt className="text-sm font-medium text-muted-foreground">Order ID</dt>
                <dd className="text-sm font-semibold">{order.id}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-muted-foreground">Order Date</dt>
                <dd className="text-sm font-semibold">{new Date(order.orderDate).toLocaleDateString()}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-muted-foreground">Total Amount</dt>
                <dd className="text-sm font-semibold">${order.totalAmount.toFixed(2)}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-muted-foreground">Items</dt>
                <dd className="text-sm font-semibold">{order.products.length}</dd>
              </div>
            </dl>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Customer Information</CardTitle>
          </CardHeader>
          <CardContent>
            {customer ? (
              <dl className="grid grid-cols-2 gap-4">
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">Name</dt>
                  <dd className="text-sm font-semibold">{customer.name}</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">Email</dt>
                  <dd className="text-sm font-semibold">{customer.email}</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">Customer ID</dt>
                  <dd className="text-sm font-semibold">{customer.id}</dd>
                </div>
              </dl>
            ) : (
              <p className="text-muted-foreground">Customer information not available</p>
            )}
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Order Items</CardTitle>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Product</TableHead>
                <TableHead className="text-right">Price</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {order.products.map((product, index) => (
                <TableRow key={index}>
                  <TableCell>{product.name}</TableCell>
                  <TableCell className="text-right">${product.price.toFixed(2)}</TableCell>
                </TableRow>
              ))}
              <TableRow>
                <TableCell className="font-bold">Total</TableCell>
                <TableCell className="text-right font-bold">${order.totalAmount.toFixed(2)}</TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  )
}
