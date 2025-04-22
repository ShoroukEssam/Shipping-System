import { Pipe, PipeTransform } from '@angular/core';
import { Delivery } from '../Models/Delivery/delivery';

@Pipe({
  name: 'searchDelivery',
  standalone: true,
  pure: false
})
export class SearchDeliveryPipe implements PipeTransform {

  // transform(delivery: Delivery[], term: string): Delivery {
  //   return delivery.filter((delivery=>delivery.name.toLocaleLowerCase().includes(term.toLocaleLowerCase())))[0];
  // }
  transform(deliveries: Delivery[], term: string): Delivery[] {
    if (!term.trim()) {
      return deliveries;
    }
    term = term.toLowerCase();
    return deliveries.filter(delivery =>
      delivery.name.toLowerCase().includes(term) ||
      delivery.email.toLowerCase().includes(term) ||
      delivery.phone.toLowerCase().includes(term) ||
      delivery.branchName.toLowerCase().includes(term)
    );
  }

}
