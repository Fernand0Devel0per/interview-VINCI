import { getOrders, getCustomers } from "@/lib/api-server"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { OrderActions } from "./order-actions"
import { CreateOrderButton } from "./create-order-button"
import { CustomerFilter } from "./customer-filter"

export default async function OrdersPage() {
  const orders = (await getOrders()) ?? []
  const customers = (await getCustomers()) ?? []

  return (
    <div className="space-y-4">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold tracking-tight">Pedidos</h1>
        <CreateOrderButton />
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Pedidos</CardTitle>
        </CardHeader>
        <div className="px-6 py-2 border-t">
          <CustomerFilter customers={customers} />
        </div>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>ID do Pedido</TableHead>
                <TableHead>Cliente</TableHead>
                <TableHead>Data</TableHead>
                <TableHead>Valor Total</TableHead>
                <TableHead>Produtos</TableHead>
                <TableHead className="text-right">Ações</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {orders.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} className="text-center">
                    Nenhum pedido encontrado
                  </TableCell>
                </TableRow>
              ) : (
                orders.map((order) => {
                  const customer = customers.find((c) => c.id === order.customerId)
                  return (
                    <TableRow key={order.id} className="customer-row" data-customer-id={order.customerId}>
                      <TableCell className="font-medium">{order.id.substring(0, 8)}...</TableCell>
                      <TableCell>{customer?.name || "Cliente desconhecido"}</TableCell>
                      <TableCell>{new Date(order.orderDate).toLocaleDateString()}</TableCell>
                      <TableCell>R$ {order.totalAmount.toFixed(2)}</TableCell>
                      <TableCell>{order.products.length} itens</TableCell>
                      <TableCell className="text-right">
                        <OrderActions order={order} />
                      </TableCell>
                    </TableRow>
                  )
                })
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  )
}
