"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Pencil, Trash2 } from "lucide-react"
import { EditProductDialog } from "./edit-product-dialog"
import { deleteProduct } from "@/lib/api-server"
import { useToast } from "@/components/ui/use-toast"
import type { Product } from "@/lib/api-server"

interface ProductActionsProps {
  product: Product
}

export function ProductActions({ product }: ProductActionsProps) {
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false)
  const { toast } = useToast()

  const handleDelete = async () => {
    if (!confirm("Tem certeza que deseja excluir este produto?")) return

    try {
      await deleteProduct(product.id)
      toast({
        title: "Sucesso",
        description: "Produto excluído com sucesso",
      })
    } catch (error) {
      toast({
        title: "Erro",
        description: error instanceof Error ? error.message : "Falha ao excluir produto",
        variant: "destructive",
      })
    }
  }

  return (
    <>
      <Button variant="ghost" size="icon" onClick={() => setIsEditDialogOpen(true)}>
        <Pencil className="h-4 w-4" />
      </Button>
      <Button variant="ghost" size="icon" onClick={handleDelete}>
        <Trash2 className="h-4 w-4" />
      </Button>

      <EditProductDialog product={product} open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen} />
    </>
  )
}
