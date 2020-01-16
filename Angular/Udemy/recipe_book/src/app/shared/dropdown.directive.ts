import { Directive, HostListener, HostBinding } from '@angular/core';

@Directive({
  selector: '[appDropdown]'
})
export class DropdownDirective {

  @HostBinding('class.open') isOpen = false;

  @HostListener('click') addOpen() {
    this.isOpen = !this.isOpen;
    console.log("Directive fired")
  }

  // @HostListener('mouseout') removeOpen() {
  //   this.isOpen = false;
  //   console.log("Directive blure fired")
  // }
  
}
