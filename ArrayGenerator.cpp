#include <iostream>
#include <vector>
#include <algorithm>
#include <random>
#include <ctime>

class ArrayGenerator {
private:
    std::vector<int> randomArray;
    std::vector<int> reversedArray;
    std::vector<int> nearlySortedArray;
    int maxSize;
    int valueRange;

public:
    ArrayGenerator(int maxSize, int valueRange)
        : maxSize(maxSize), valueRange(valueRange) {
        generateRandomArray();
        generateReversedArray();
        generateNearlySortedArray();
    }

    void generateRandomArray() {
        randomArray.resize(maxSize);
        std::mt19937 rng(static_cast<unsigned>(time(nullptr)));
        std::uniform_int_distribution<int> dist(0, valueRange);

        for (int i = 0; i < maxSize; ++i) {
            randomArray[i] = dist(rng);
        }
    }

    void generateReversedArray() {
        reversedArray.resize(maxSize);
        for (int i = 0; i < maxSize; ++i) {
            reversedArray[i] = maxSize - i;
        }
    }

    void generateNearlySortedArray() {
        nearlySortedArray.resize(maxSize);
        for (int i = 0; i < maxSize; ++i) {
            nearlySortedArray[i] = i;
        }
        const size_t gap = 15;
        for (int i = 0; i < maxSize / 50; ++i) {
            size_t secPlace;
            size_t x = std::rand() % (maxSize + 1) - gap;
            secPlace = x + gap;
            std::swap(nearlySortedArray[x], nearlySortedArray[secPlace]);
        }
    }

    std::vector<int> getArray(const std::string &type, int size) const {
        if (type == "random") {
            return std::vector<int>(randomArray.begin(), randomArray.begin() + size);
        }
        if (type == "reversed") {
            return std::vector<int>(reversedArray.begin(), reversedArray.begin() + size);
        }
        if (type == "nearly_sorted") {
            return std::vector<int>(nearlySortedArray.begin(), nearlySortedArray.begin() + size);
        }
        throw std::invalid_argument("Invalid array type");
    }
};
