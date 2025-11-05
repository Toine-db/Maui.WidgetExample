//
//  MyWidgetEntryView.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

struct MyWidgetEntryView : View {
    var entry: Provider.Entry

    var body: some View {
        VStack {
            Text("Time:")
            Text(entry.date, style: .time)

            Text("Favorite Emoji:")
            Text(entry.configuration.favoriteEmoji)
        }
    }
}
