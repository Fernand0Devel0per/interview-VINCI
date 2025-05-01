"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Plus } from "lucide-react"
import { CreateOrderDialog } from "./create-order-dialog"

export function CreateOrderButton() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  return (
    <>
      <Button onClick={() => setIsDialogOpen(true)}>
        <Plus className="mr-2 h-4 w-4" />
        Criar Pedido
      </Button>

      <CreateOrderDialog open={isDialogOpen} onOpenChange={setIsDialogOpen} />
    </>
  )
}
