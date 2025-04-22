import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from './../../Services/order.service';
import { IOrder } from '../../Models/order/order';
import { PdfGeneratorService } from '../../Services/pdf-generator.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-order-report',
  templateUrl: './order-report.component.html',
  styleUrl: './order-report.component.css'
})
export class OrderReportComponent implements OnInit, OnDestroy {
  @ViewChild('reportContent') reportContent!: ElementRef;
  orderId: number | null = null;
  order:IOrder = {} as IOrder;
  orderSubscription: any;
  constructor(
    private orderService: OrderService,
    private pdfGeneratorService: PdfGeneratorService,
    private route: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.orderId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.orderId && this.orderId !== 0) {
      this.orderSubscription = this.orderService.getOrderReceipt(this.orderId).subscribe({
        next: (order) => {
          this.order = order;
        },
        error: (err) => {
          console.error(err.message);
          Swal.fire(
            'خطأ!',
            'حدث خطأ في عرض التقرير',
            'error'
          );
        }
      });
    }
  }
  printReport() {
    this.pdfGeneratorService.generatePagePdf(this.reportContent.nativeElement, 'Order Report');
  }
  ngOnDestroy(): void {
    if (this.orderSubscription) {
      this.orderSubscription.unsubscribe();
    }
  }
}
