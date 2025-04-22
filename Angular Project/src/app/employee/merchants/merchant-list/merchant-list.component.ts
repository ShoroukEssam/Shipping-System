import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../shared/Services/api.service';
import { Router } from '@angular/router';
import { IMerchantDTO } from '../../../shared/Models/IMerchant';
import Swal from 'sweetalert2';
import { MerchantService } from './../../../shared/Services/merchant.service';

@Component({
  selector: 'app-merchant-list',
  templateUrl: './merchant-list.component.html',
  styleUrl: './merchant-list.component.css'
})
export class MerchantListComponent implements OnInit {

merchants:IMerchantDTO[]=[];

loading : boolean = false;

  constructor(private apiService:ApiService,private router:Router, private merchantService:MerchantService){}

  ngOnInit(): void {
    this.loading = true;
    this.merchantService.getAllMerchants().subscribe({
      next:(res:any)=>{
        console.log(res);
       // this.merchants=res
       this.merchants=res.$values;
       this.loading = false;
      },
      error:(err)=>{
        console.log(err)
        this.loading = false;
      }
    })
    // this.apiService.get<any>('/Merchant').subscribe({
    //   next:(res)=>{
    //       console.log(res);
    //    // this.merchants=res
    //    this.merchants=this.merchantsForTest;
    //    this.loading = false;
    //   },
    //   error:(err)=>{
    //     Swal.fire(
    //       'عرض !',
    //       'حدث خطأ في عرض التجار',
    //       'error'
    //     );
    //     console.log(err)
    //     this.loading = false;
    //   }
    // })
  }


  onSearch(searchText:string){
    if (!searchText.trim()) {
      this.ngOnInit();
      return;
    }
    this.merchants = this.merchants.filter(merchant => merchant.name.toLowerCase().includes(searchText.toLowerCase()));
  }

  deleteMerchant(id:string|undefined): void {
    Swal.fire({
      title: 'هل انت متأكد',
      text: 'سيتم حذف هذا التاجر',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم, قم بالحذف',
      cancelButtonText: 'لا, الغاء',
    }).then((result) => {
      if (result.isConfirmed) {
        this.apiService.delete<any>(`/Merchant/DeleteMerchant?id=${id}`).subscribe({
          next: (res) => {
            this.merchants = this.merchants.filter(p => p.id !== id);
            Swal.fire(
              'حذف التاجر!',
              'تم حذف هذا التاجر.',
              'success'
            );
          },
          error: (error: any) => {
            this.merchants = this.merchants.filter(p => p.id !== id);
            Swal.fire(
              'حذف التاجر!',
              'تم حذف هذا التاجر.',
              'success'
            );
          }
        });
      }
    });
  }




  toggleStateStatus(merchant: IMerchantDTO): void {
    merchant.status = !merchant.status;
    this.apiService.put<any, any>( `/Merchant/UpdateStatus?id=${merchant.id}&status=${merchant.status}`, merchant).subscribe({
      next: () => {
        Swal.fire(
          'تحديث الحالة!',
          'تم تحديث حالة التاجر بنجاح.',
          'success'
        );
      },
      error: (error: any) => {
        Swal.fire(
          'تحديث الحالة!',
          `حدث خطأ أثناء تحديث الحالة: ${error.error}`,
          'error'
        );
        console.error('Error updating state status:', error);
      }
    });
  }







}
