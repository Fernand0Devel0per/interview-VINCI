"use client"

import { Button } from "@/components/ui/button"
import { Eye, Trash2 } from "lucide-react"
import { deleteOrder } from "@/lib/api-server"
import { useToast } from "@/components/ui/use-toast"
import type { Order } from "@/lib/api-server"
import Link from "next/link"

interface OrderActionsProps {
  order: Order
}

export function OrderActions({ order }: OrderActionsProps) {
  const { toast } = useToast()

  const handleDelete = async () => {
    if (!confirm("Tem certeza que deseja excluir este pedido?")) return

    try {
      await deleteOrder(order.id)
      toast({
        title: "Sucesso",
        description: "Pedido excluído com sucesso",
      })
    } catch (error) {
      toast({
        title: "Erro",
        description: error instanceof Error ? error.message : "Falha ao excluir pedido",
        variant: "destructive",
      })
    }
  }

  return (
    <>
      <Link href={`/orders/${order.id}`}>
        <Button variant="ghost" size="icon">
          <Eye className="h-4 w-4" />
        </Button>
      </Link>
      <Button variant="ghost" size="icon" onClick={handleDelete}>
        <Trash2 className="h-4 w-4" />
      </Button>
    </>
  )
}
