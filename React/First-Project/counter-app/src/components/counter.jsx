import React, { Component } from "react";

export class Counter extends Component {
  render() {
    const { onDelete, onIncrement, onDecrement, counter } = this.props;

    return (
      <div className="row">
        <div className="col-1">
          <span className={this.getBadgeClass()}>{this.formatCount()}</span>
        </div>
        <button
          onClick={() => onIncrement(counter)}
          className=" btn btn-secondary btn-sm m-2"
        >
          +
        </button>
        <button
          onClick={() => onDecrement(counter)}
          className="btn btn-secondary btn-sm m-2"
          disabled={counter.value === 0 ? "disabled" : ""}
        >
          -
        </button>
        <button
          className=" btn btn-danger btn-sm m-2"
          onClick={() => onDelete(counter.id)}
        >
          X
        </button>
        <div className="col"></div>
      </div>
    );
  }
  getBadgeClass() {
    let classes = " badge m-2 badge-";
    classes += this.props.counter.value === 0 ? "warning" : "primary";
    return classes;
  }

  formatCount() {
    const { value: count } = this.props.counter;
    return count === 0 ? "Zero" : count;
  }
}

export default Counter;
