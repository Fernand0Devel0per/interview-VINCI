"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Plus } from "lucide-react"
import { CreateProductDialog } from "./create-product-dialog"

export function CreateProductButton() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  return (
    <>
      <Button onClick={() => setIsDialogOpen(true)}>
        <Plus className="mr-2 h-4 w-4" />
        Adicionar Produto
      </Button>

      <CreateProductDialog open={isDialogOpen} onOpenChange={setIsDialogOpen} />
    </>
  )
}
