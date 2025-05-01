import { getOrderById, getCustomerById } from "@/lib/api-server"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { DeleteOrderButton } from "./delete-order-button"
import { ArrowLeft } from "lucide-react"
import Link from "next/link"
import { notFound } from "next/navigation"

export default async function OrderDetailsPage({ params }: { params: { id: string } }) {
  try {
    const order = await getOrderById(params.id)

    let customer = null
    try {
      customer = await getCustomerById(order.customerId)
    } catch (error) {
      console.error("Custumer not found:", error)
    }

    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Link href="/orders">
              <div className="flex items-center justify-center w-10 h-10 rounded-full border hover:bg-muted">
                <ArrowLeft className="h-4 w-4" />
              </div>
            </Link>
            <h1 className="text-3xl font-bold tracking-tight">Detalhes do Pedido</h1>
          </div>
          <DeleteOrderButton orderId={order.id} />
        </div>

        <div className="grid gap-6 md:grid-cols-2">
          <Card>
            <CardHeader>
              <CardTitle>Informações do Pedido</CardTitle>
            </CardHeader>
            <CardContent>
              <dl className="grid grid-cols-2 gap-4">
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">ID do Pedido</dt>
                  <dd className="text-sm font-semibold">{order.id}</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">Data do Pedido</dt>
                  <dd className="text-sm font-semibold">{new Date(order.orderDate).toLocaleDateString()}</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">Valor Total</dt>
                  <dd className="text-sm font-semibold">R$ {order.totalAmount.toFixed(2)}</dd>
                </div>
                <div>
                  <dt className="text-sm font-medium text-muted-foreground">Itens</dt>
                  <dd className="text-sm font-semibold">{order.products.length}</dd>
                </div>
              </dl>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Informações do Cliente</CardTitle>
            </CardHeader>
            <CardContent>
              {customer ? (
                <dl className="grid grid-cols-2 gap-4">
                  <div>
                    <dt className="text-sm font-medium text-muted-foreground">Nome</dt>
                    <dd className="text-sm font-semibold">{customer.name}</dd>
                  </div>
                  <div>
                    <dt className="text-sm font-medium text-muted-foreground">Email</dt>
                    <dd className="text-sm font-semibold">{customer.email}</dd>
                  </div>
                  <div>
                    <dt className="text-sm font-medium text-muted-foreground">ID do Cliente</dt>
                    <dd className="text-sm font-semibold">{customer.id}</dd>
                  </div>
                </dl>
              ) : (
                <p className="text-muted-foreground">Informações do cliente não disponíveis</p>
              )}
            </CardContent>
          </Card>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>Itens do Pedido</CardTitle>
          </CardHeader>
          <CardContent>
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Produto</TableHead>
                  <TableHead className="text-right">Preço</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {order.products.map((product, index) => (
                  <TableRow key={index}>
                    <TableCell>{product.name}</TableCell>
                    <TableCell className="text-right">R$ {product.price.toFixed(2)}</TableCell>
                  </TableRow>
                ))}
                <TableRow>
                  <TableCell className="font-bold">Total</TableCell>
                  <TableCell className="text-right font-bold">R$ {order.totalAmount.toFixed(2)}</TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </CardContent>
        </Card>
      </div>
    )
  } catch (error) {
    notFound()
  }
}
