import onnxruntime as ort
import numpy as np
import sys
import json


def main():
    model_path = sys.argv[1]
    input_data = json.loads(sys.argv[2])
    sess = ort.InferenceSession(model_path)
    input_name = sess.get_inputs()[0].name
    test_data = np.array(input_data, dtype=np.float32).reshape(1, -1)
    outputs = sess.run(None, {input_name: test_data})
    predicted_class = np.argmax(outputs[0])
    probabilities = outputs[0].flatten()
    print(json.dumps({"class": int(predicted_class), "probabilities": probabilities.tolist()}))


if __name__ == "__main__":
    main()
