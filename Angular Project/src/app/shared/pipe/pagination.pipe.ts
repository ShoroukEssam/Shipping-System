import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pagination',
})
export class PaginationPipe implements PipeTransform {
  transform(items: any[], limit: number): any[] {
    if (!items || !limit) {
      return items;
    }
    return items.slice(0, limit);
  }
}
