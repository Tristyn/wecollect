pragma solidity ^0.4.24;

contract test {
    int _multiplier;

    constructor(int multipier) public {
        _multiplier = multipier;
    }

    function mulitply(int val) public view returns (int d){
        return val * _multiplier;
    }
}