# Performance

# Take Away: Measure!

# Why? Game: 60FPS, 16ms, GPU rendering

# How? BenchmarkDotNet: micro-benchmark, instruction timing, allocation

- Random insert/remove between linked list and array
- Json.NET vs protobuf
- Docfx regex vs manual matcher
- Linq
- Binary Search vs Sequential Scan
- Dictionary Lookup vs Sequential Scan

# CPU

- Zero Cost Abstraction (Rust and C++): Generic structual, [spoiler] shapes
- Parallel:
- Vectorization: SIMD instructions using Vector<T>


# Memory

- Speed: CPU L1/L2/L3, Memory, Disk, Network
- Memory Model: Stack + Heap (Gen0, Gen1, Gen2, LOH)

    - GC Basics: ref graph, roots, weak ref, mark & sweap, gc stops with world, concurrent gc
    - Stack: [spoiler] value types, struct IDisposable, struct Enumerator, span<T>, 
    - Gen0: thread-static vs new
    - Pinning:
    - LOH: Native Buffer Bump


# Examples

- Example 1: [Starvation] Open Localization Linq Memory Leak (Dump)

    - ToList() - growing
    - ToArray() - growing + copy, even worse 

- Example 2: [Linq] Open Localization Linq/Struct Slow

    - Large Struct comparision / Copying
    - Linq lazy evaluation -> algorithm complexity

- Example 3: [Pinning] Docs Rendering Memory Leak (Dump)

    - Dump analysis
    - Static, thread local static, async local static
    - Immutable collections (thread-safety + immutability boundaries)

