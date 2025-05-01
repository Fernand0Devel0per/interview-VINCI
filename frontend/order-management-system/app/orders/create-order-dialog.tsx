"use client"

import { useState, useEffect } from "react"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Checkbox } from "@/components/ui/checkbox"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { createOrder, getCustomers, getProducts } from "@/lib/api-server"
import { useToast } from "@/components/ui/use-toast"
import type { Customer, Product } from "@/lib/api-server"

interface CreateOrderDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
}

export function CreateOrderDialog({ open, onOpenChange }: CreateOrderDialogProps) {
  const [customers, setCustomers] = useState<Customer[]>([])
  const [products, setProducts] = useState<Product[]>([])
  const [loading, setLoading] = useState(true)
  const [selectedCustomerId, setSelectedCustomerId] = useState("")
  const [selectedProductIds, setSelectedProductIds] = useState<string[]>([])
  const { toast } = useToast()

  useEffect(() => {
    if (open) {
      const fetchData = async () => {
        try {
          setLoading(true)
          const [customersData, productsData] = await Promise.all([getCustomers(), getProducts()])
          setCustomers(customersData)
          setProducts(productsData)
        } catch (error) {
          toast({
            title: "Erro",
            description: "Falha ao carregar dados",
            variant: "destructive",
          })
        } finally {
          setLoading(false)
        }
      }

      fetchData()
    }
  }, [open, toast])

  const handleCustomerChange = (value: string) => {
    setSelectedCustomerId(value)
  }

  const handleProductToggle = (productId: string) => {
    setSelectedProductIds((prev) =>
      prev.includes(productId) ? prev.filter((id) => id !== productId) : [...prev, productId],
    )
  }

  const handleSubmit = async () => {
    if (!selectedCustomerId || selectedProductIds.length === 0) {
      toast({
        title: "Erro",
        description: "Selecione um cliente e pelo menos um produto",
        variant: "destructive",
      })
      return
    }

    try {
      await createOrder(selectedCustomerId, selectedProductIds)
      toast({
        title: "Sucesso",
        description: "Pedido criado com sucesso",
      })
      onOpenChange(false)
      setSelectedCustomerId("")
      setSelectedProductIds([])
    } catch (error) {
      toast({
        title: "Erro",
        description: error instanceof Error ? error.message : "Falha ao criar pedido",
        variant: "destructive",
      })
    }
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Criar Pedido</DialogTitle>
          <DialogDescription>Crie um novo pedido selecionando um cliente e produtos</DialogDescription>
        </DialogHeader>

        {loading ? (
          <div className="py-6 text-center">Carregando...</div>
        ) : (
          <div className="grid gap-4 py-4">
            <div className="grid gap-2">
              <Label htmlFor="customer">Cliente</Label>
              <Select value={selectedCustomerId} onValueChange={handleCustomerChange}>
                <SelectTrigger id="customer">
                  <SelectValue placeholder="Selecione um cliente" />
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
              <Label>Produtos</Label>
              <div className="border rounded-md p-4 max-h-[200px] overflow-y-auto">
                {products.length === 0 ? (
                  <p className="text-sm text-muted-foreground">Nenhum produto disponível</p>
                ) : (
                  products.map((product) => (
                    <div key={product.id} className="flex items-center space-x-2 py-2">
                      <Checkbox
                        id={`product-${product.id}`}
                        checked={selectedProductIds.includes(product.id)}
                        onCheckedChange={() => handleProductToggle(product.id)}
                      />
                      <Label htmlFor={`product-${product.id}`} className="flex-1">
                        {product.name} - R$ {product.price.toFixed(2)}
                      </Label>
                    </div>
                  ))
                )}
              </div>
            </div>
          </div>
        )}

        <DialogFooter>
          <Button onClick={handleSubmit} disabled={loading}>
            Criar Pedido
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
