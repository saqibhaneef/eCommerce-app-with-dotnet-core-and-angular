import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements ControlValueAccessor {
@Input() type='text';
 @Input() label='';

 constructor(@Self() public contrlDir:NgControl) {
  this.contrlDir.valueAccessor=this;
 }

  writeValue(obj: any): void {
    
  }
  registerOnChange(fn: any): void {
    
  }
  registerOnTouched(fn: any): void {
  }
  
  get control():FormControl{
    return this.contrlDir.control as FormControl;
  }

}
