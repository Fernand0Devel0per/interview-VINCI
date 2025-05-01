"use client"

import { Button } from "@/components/ui/button"
import { Trash2 } from "lucide-react"
import { deleteOrder } from "@/lib/api-server"
import { useToast } from "@/components/ui/use-toast"
import { useRouter } from "next/navigation"

interface DeleteOrderButtonProps {
  orderId: string
}

export function DeleteOrderButton({ orderId }: DeleteOrderButtonProps) {
  const { toast } = useToast()
  const router = useRouter()

  const handleDelete = async () => {
    if (!confirm("Tem certeza que deseja excluir este pedido?")) return

    try {
      await deleteOrder(orderId)
      toast({
        title: "Sucesso",
        description: "Pedido excluído com sucesso",
      })
      router.push("/orders")
    } catch (error) {
      toast({
        title: "Erro",
        description: error instanceof Error ? error.message : "Falha ao excluir pedido",
        variant: "destructive",
      })
    }
  }

  return (
    <Button variant="destructive" onClick={handleDelete}>
      <Trash2 className="mr-2 h-4 w-4" />
      Excluir Pedido
    </Button>
  )
}
